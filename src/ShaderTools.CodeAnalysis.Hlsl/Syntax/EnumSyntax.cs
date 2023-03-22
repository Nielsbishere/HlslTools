using System.Collections.Generic;

namespace ShaderTools.CodeAnalysis.Hlsl.Syntax
{
    public sealed partial class EnumSyntax : TypeDefinitionSyntax
    {
        public bool isClass;

        public EnumSyntax(SyntaxToken enumKeyword, bool isClass, SyntaxToken name, BaseListSyntax baseList, SyntaxToken openBraceToken, List<SyntaxNode> members, SyntaxToken closeBraceToken)
            : this(SyntaxKind.EnumKeyword, enumKeyword, name, baseList, openBraceToken, members, closeBraceToken)
        {
			this.isClass = isClass;
        }
    }
}