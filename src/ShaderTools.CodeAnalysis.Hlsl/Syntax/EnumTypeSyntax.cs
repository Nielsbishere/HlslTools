using System.Collections.Generic;

namespace ShaderTools.CodeAnalysis.Hlsl.Syntax
{
    public sealed partial class EnumTypeSyntax : TypeDefinitionSyntax
    {
        public EnumTypeSyntax(SyntaxToken enumKeyword, SyntaxToken classKeyword, SyntaxToken name, SyntaxToken colonToken, IdentifierNameSyntax baseType, SyntaxToken openBraceToken, List<VariableDeclarationStatementSyntax> members, SyntaxToken closeBraceToken)
            : this(SyntaxKind.EnumType,
                   enumKeyword, classKeyword, name, colonToken, baseType, openBraceToken, members, closeBraceToken)
        {
        }
    }
}