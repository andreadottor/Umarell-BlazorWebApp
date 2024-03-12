namespace Dottor.Umarell.Client.Exceptions;

using System;

public class WrongParentException : Exception
{
    public WrongParentException(Type componentType, Type parentType)
    {
        ComponentType = componentType;
        ParentType = parentType;
    }

    public Type ComponentType { get; set; }

    public Type ParentType { get; set; }

    public override string Message => $"Componennt {ComponentType} must be in a {ParentType}";
}
