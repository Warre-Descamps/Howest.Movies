using System.Reflection;

namespace Magyx.Common.Extensions;

public static class TypeExtenions
{
    private static NullabilityInfoContext? _nullabilityInfoContext;
    
    public static bool IsNullable(this ParameterInfo info)
    {
        if ((_nullabilityInfoContext ??= new NullabilityInfoContext()).Create(info).WriteState is NullabilityState.Nullable)
            return true;
        return false;
    }
}