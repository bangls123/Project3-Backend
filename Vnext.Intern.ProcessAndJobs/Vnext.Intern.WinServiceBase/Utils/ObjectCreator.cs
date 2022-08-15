using System;

namespace Vnext.Intern.WinServiceBase.Utils
{
    public class ObjectCreator
    {
        public static object CreateInstance(string typeName)
        {
            var t = Type.GetType(typeName);
            if (t == null)
                throw new TypeNotFoundException(typeName);

            var obj = t.GetConstructor(new Type[] { })?.Invoke(new object[] { });
            if (obj == null)
                throw new TypeInstatiationException(typeName);

            return obj;
        }
    }
}
