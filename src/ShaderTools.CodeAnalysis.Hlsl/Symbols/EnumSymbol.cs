using System.Collections.Generic;
using System.Collections.Immutable;
using ShaderTools.CodeAnalysis.Hlsl.Binding;
using ShaderTools.CodeAnalysis.Hlsl.Syntax;
using ShaderTools.CodeAnalysis.Symbols;
using ShaderTools.CodeAnalysis.Syntax;
using ShaderTools.CodeAnalysis.Text;

namespace ShaderTools.CodeAnalysis.Hlsl.Symbols
{
    public sealed class EnumSymbol : TypeSymbol, INamedTypeSymbol
    {
        public ScalarType BaseType { get; }

        public EnumTypeSyntax Syntax { get; }

		public bool IsEnumClass { get; }

        public override SyntaxTreeBase SourceTree => Syntax.SyntaxTree;
        public override ImmutableArray<SourceRange> Locations { get; }
        public override ImmutableArray<SyntaxNodeBase> DeclaringSyntaxNodes { get; }

        internal EnumSymbol(EnumTypeSyntax syntax, Symbol parent, ScalarType baseType, Binder binder)
            : base(SymbolKind.Enum, syntax.Name.Text, string.Empty, parent)
        {
            Syntax = syntax;
            BaseType = baseType;
            Binder = binder;
			IsEnumClass = syntax.ClassKeyword != null;

            Locations = syntax.Name != null
                ? ImmutableArray.Create(syntax.Name.SourceRange)
                : ImmutableArray<SourceRange>.Empty;

            DeclaringSyntaxNodes = ImmutableArray.Create((SyntaxNodeBase) syntax);
        }

        public override IEnumerable<T> LookupMembers<T>(string name)
        {
            return base.LookupMembers<T>(name);
        }

        private bool Equals(StructSymbol other)
        {
            return base.Equals(other) && Equals(BaseType, other.BaseType);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            return obj.GetType() == GetType() && Equals((EnumSymbol)obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                int hashCode = base.GetHashCode();
                return (hashCode * 397) ^ (int) BaseType;
            }
        }
    }
}