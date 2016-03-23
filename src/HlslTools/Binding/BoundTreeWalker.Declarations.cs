﻿using System;
using HlslTools.Binding.BoundNodes;

namespace HlslTools.Binding
{
    internal abstract partial class BoundTreeWalker
    {
        protected virtual void VisitTopLevelDeclaration(BoundNode node)
        {
            switch (node.Kind)
            {
                case BoundNodeKind.VariableDeclaration:
                    VisitVariableDeclaration((BoundVariableDeclaration) node);
                    break;
                case BoundNodeKind.MultipleVariableDeclarations:
                    VisitMultipleVariableDeclarations((BoundMultipleVariableDeclarations) node);
                    break;
                case BoundNodeKind.FunctionDeclaration:
                    VisitFunctionDeclaration((BoundFunctionDeclaration) node);
                    break;
                case BoundNodeKind.FunctionDefinition:
                    VisitFunctionDefinition((BoundFunctionDefinition) node);
                    break;
                case BoundNodeKind.ConstantBuffer:
                    VisitConstantBuffer((BoundConstantBuffer) node);
                    break;
                case BoundNodeKind.TypeDeclaration:
                    VisitTypeDeclaration((BoundTypeDeclaration) node);
                    break;
                case BoundNodeKind.Namespace:
                    VisitNamespace((BoundNamespace) node);
                    break;
                case BoundNodeKind.Technique:
                    VisitTechnique((BoundTechnique) node);
                    break;
                default:
                    throw new InvalidOperationException(node.Kind.ToString());
            }
        }

        protected virtual void VisitMultipleVariableDeclarations(BoundMultipleVariableDeclarations node)
        {
            foreach (var declaration in node.VariableDeclarations)
                VisitVariableDeclaration(declaration);
        }

        protected virtual void VisitVariableDeclaration(BoundVariableDeclaration node)
        {
            if (node.InitializerOpt != null)
                VisitInitializer(node.InitializerOpt);
        }

        protected virtual void VisitFunctionDeclaration(BoundFunctionDeclaration node)
        {
            foreach (var parameter in node.Parameters)
                VisitVariableDeclaration(parameter);
        }

        protected virtual void VisitFunctionDefinition(BoundFunctionDefinition node)
        {
            foreach (var parameter in node.Parameters)
                VisitVariableDeclaration(parameter);
            VisitBlock(node.Body);
        }

        protected virtual void VisitConstantBuffer(BoundConstantBuffer node)
        {
            foreach (var declaration in node.Variables)
                VisitMultipleVariableDeclarations(declaration);
        }

        protected virtual void VisitTypeDeclaration(BoundTypeDeclaration node)
        {
            VisitType(node.BoundType);
        }

        protected virtual void VisitType(BoundType node)
        {
            switch (node.Kind)
            {
                case BoundNodeKind.StructType:
                    VisitStructType((BoundStructType) node);
                    break;
                case BoundNodeKind.ClassType:
                    VisitClassType((BoundClassType) node);
                    break;
                case BoundNodeKind.InterfaceType:
                    VisitInterfaceType((BoundInterfaceType) node);
                    break;
                default:
                    throw new InvalidOperationException(node.Kind.ToString());
            }
        }

        protected virtual void VisitStructType(BoundStructType node)
        {
            foreach (var declaration in node.Variables)
                VisitMultipleVariableDeclarations(declaration);
        }

        protected virtual void VisitClassType(BoundClassType node)
        {
            foreach (var member in node.Members)
            {
                switch (member.Kind)
                {
                    case BoundNodeKind.MultipleVariableDeclarations:
                        VisitMultipleVariableDeclarations((BoundMultipleVariableDeclarations) member);
                        break;
                    case BoundNodeKind.FunctionDeclaration:
                        VisitFunctionDeclaration((BoundFunctionDeclaration) member);
                        break;
                    case BoundNodeKind.FunctionDefinition:
                        VisitFunctionDefinition((BoundFunctionDefinition) member);
                        break;
                }
            }
        }

        protected virtual void VisitInterfaceType(BoundInterfaceType node)
        {
            foreach (var method in node.Methods)
                VisitFunction(method);
        }

        protected virtual void VisitFunction(BoundFunction node)
        {
            switch (node.Kind)
            {
                case BoundNodeKind.FunctionDeclaration:
                    VisitFunctionDeclaration((BoundFunctionDeclaration) node);
                    break;
                case BoundNodeKind.FunctionDefinition:
                    VisitFunctionDefinition((BoundFunctionDefinition) node);
                    break;
                default:
                    throw new InvalidOperationException(node.Kind.ToString());
            }
        }

        protected virtual void VisitNamespace(BoundNamespace node)
        {
            foreach (var declaration in node.Declarations)
                VisitTopLevelDeclaration(declaration);
        }

        protected virtual void VisitTechnique(BoundTechnique node)
        {
            foreach (var pass in node.Passes)
                VisitPass(pass);
        }

        protected virtual void VisitPass(BoundPass node)
        {
            
        }
    }
}