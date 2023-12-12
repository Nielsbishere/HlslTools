using System.Collections.Immutable;
using ShaderTools.CodeAnalysis.Hlsl.Symbols;

namespace ShaderTools.CodeAnalysis.Hlsl.Binding.BoundNodes
{
    internal sealed class BoundEnumType : BoundType
    {
        public EnumSymbol EnumSymbol { get; }
        public ImmutableArray<BoundNode> Members { get; }

        public BoundEnumType(EnumSymbol enumSymbol, ImmutableArray<BoundNode> members)
            : base(BoundNodeKind.EnumType, enumSymbol)
        {
			EnumSymbol = enumSymbol;
            Members = members;
        }
    }
}