﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading;
using System.Xml;
using System.Xml.Serialization;
using Microsoft.Build.Framework;
using Microsoft.Build.Utilities;
using SyntaxGenerator.Model;
using SyntaxGenerator.Writer;

namespace SyntaxGenerator
{
    public class GenerateSyntaxTask : AppDomainIsolatedTask, ICancelableTask
    {
        private readonly CancellationTokenSource _cts = new CancellationTokenSource();

        [Required]
        public ITaskItem[] Compile { get; set; }

        [Required]
        public string IntermediateOutputDirectory { get; set; }

        [Output]
        public ITaskItem[] GeneratedCompile { get; set; }

        [Output]
        public ITaskItem[] AdditionalWrittenFiles { get; set; }

        public override bool Execute()
        {
            try
            {
                ExecuteInternal();
                return !Log.HasLoggedErrors;
            }
            catch (OperationCanceledException)
            {
                Log.LogMessage(MessageImportance.High, "Canceled.");
                return false;
            }
        }

        private void ExecuteInternal()
        {
            System.Threading.Tasks.Task.Run(() =>
            {
                var outputFiles = new List<ITaskItem>();
                var writtenFiles = new List<ITaskItem>();

                foreach (var sourceFile in Compile)
                {
                    var reader = XmlReader.Create(sourceFile.ItemSpec, new XmlReaderSettings { DtdProcessing = DtdProcessing.Prohibit });
                    var serializer = new XmlSerializer(typeof(Tree));
                    Tree tree = (Tree) serializer.Deserialize(reader);

                    var outputPath = IntermediateOutputDirectory;
                    var prefix = Path.GetFileName(sourceFile.ItemSpec);
                    var outputMainFile = Path.Combine(outputPath, $"{prefix}.Main.Generated.cs");
                    var outputInternalFile = Path.Combine(outputPath, $"{prefix}.Internal.Generated.cs");
                    var outputSyntaxFile = Path.Combine(outputPath, $"{prefix}.Syntax.Generated.cs");

                    _cts.Token.ThrowIfCancellationRequested();

                    WriteToFile(tree, SourceWriter.WriteMain, outputMainFile);
                    outputFiles.Add(new TaskItem(outputMainFile));

                    _cts.Token.ThrowIfCancellationRequested();

                    //WriteToFile(tree, SourceWriter.WriteInternal, outputInternalFile);
                    //outputFiles.Add(new TaskItem(outputInternalFile));

                    //_cts.Token.ThrowIfCancellationRequested();

                    WriteToFile(tree, SourceWriter.WriteSyntax, outputSyntaxFile);
                    outputFiles.Add(new TaskItem(outputSyntaxFile));

                    _cts.Token.ThrowIfCancellationRequested();
                }

                GeneratedCompile = outputFiles.ToArray();
                AdditionalWrittenFiles = writtenFiles.ToArray();
            }).GetAwaiter().GetResult();
        }

        private static void WriteToFile(Tree tree, Action<TextWriter, Tree> writeAction, string outputFile)
        {
            var stringBuilder = new StringBuilder();
            var writer = new StringWriter(stringBuilder);
            writeAction(writer, tree);

            var text = stringBuilder.ToString();
            int length;
            do
            {
                length = text.Length;
                text = text.Replace("{\r\n\r\n", "{\r\n");
            } while (text.Length != length);

            try
            {
                using (var outFile = new StreamWriter(File.Open(outputFile, FileMode.Create)))
                {
                    outFile.Write(text);
                }
            }
            catch (UnauthorizedAccessException)
            {
                Console.WriteLine("Unable to access {0}.", outputFile);
            }
        }

        void ICancelableTask.Cancel()
        {
            _cts.Cancel();
        }
    }
}