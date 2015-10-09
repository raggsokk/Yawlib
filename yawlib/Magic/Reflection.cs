using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Management;
using System.Reflection;
using System.Reflection.Emit;
using System.Diagnostics;

namespace Yawlib.Magic
{
    internal sealed class Reflection
    {
        internal delegate object CreateObject();
        internal delegate object GenericSetter(object target, object value);

        #region Singleton

        // Singleton pattern 4 from : http://csharpindepth.com/articles/general/singleton.aspx
        private static readonly Reflection instance = new Reflection();
        // Explicit static constructor to tell C# compiler
        // not to mark type as beforefieldinit
        static Reflection()
        {
        }
        private Reflection()
        {
        }
        public static Reflection Instance { get { return instance; } }

        #endregion

        #region Cache Functions

        private SortedDictionary<string, clsMyType> sTypeCache = new SortedDictionary<string, clsMyType>();
        private object sTypeCacheLock = new object();

        internal clsMyType TryGetMyType(string fullname, Type objType)
        {
            clsMyType myType = null;

            if (sTypeCache.TryGetValue(fullname, out myType))
            {
                return myType;
            }
            else
            {
                myType = new clsMyType(objType);

                lock (sTypeCacheLock)
                {
                    sTypeCache.Add(myType.FullName, myType);
                }

                return myType;
            }
        }

        private SortedDictionary<string, CreateObject> sCreateCache = new SortedDictionary<string, CreateObject>();
        private object sCreateCacheLock = new object();

        internal CreateObject TryGetCreateObject(string fullname, Type t)
        {
            CreateObject c = null;

            if (sCreateCache.TryGetValue(fullname, out c))
            {
                return c;
            }
            else
            {
                c = CompileCreateObject(t);

                lock (sCreateCacheLock)
                {
                    sCreateCache.Add(fullname, c);
                }

                return c;
            }
        }

        #endregion

        #region Reflection (could be static actually.

        internal static CreateObject CompileCreateObject(Type objType)
        {
            try
            {
                if (objType.IsClass)
                {
                    DynamicMethod dynMethod = new DynamicMethod("_", objType, null);
                    ILGenerator ilGen = dynMethod.GetILGenerator();

                    ilGen.Emit(OpCodes.Newobj, objType.GetConstructor(Type.EmptyTypes));
                    ilGen.Emit(OpCodes.Ret);
                    var c = (CreateObject)dynMethod.CreateDelegate(typeof(CreateObject));
                    return c;
                }
                else // struct
                {
                    DynamicMethod dynMethod = new DynamicMethod("_", typeof(object), null);
                    ILGenerator ilGen = dynMethod.GetILGenerator();

                    var lv = ilGen.DeclareLocal(objType);

                    ilGen.Emit(OpCodes.Ldloca_S, lv);
                    ilGen.Emit(OpCodes.Initobj, objType);
                    ilGen.Emit(OpCodes.Ldloc_0);
                    ilGen.Emit(OpCodes.Box, objType);
                    ilGen.Emit(OpCodes.Ret);
                    var c = (CreateObject)dynMethod.CreateDelegate(typeof(CreateObject));
                    return c;
                }
            }
            catch (Exception exc)
            {
                throw new Exception(string.Format("Failed to compile generic createobject method for instance '{0}' from assembly '{1}'. Doest it have a empty constructor?",
                    objType.FullName, objType.AssemblyQualifiedName), exc);
            }
        }

        internal static GenericSetter CompileGenericSetMethod(Type type, PropertyInfo prop)
        {
            try
            {
                MethodInfo setMethod = prop.GetSetMethod();
                if (setMethod == null)
                    return null;

                Type[] arguments = new Type[2];
                arguments[0] = arguments[1] = typeof(object);

                DynamicMethod dynSetter = new DynamicMethod("_", typeof(object), arguments);
                ILGenerator ilGen = dynSetter.GetILGenerator();

                if (!type.IsClass) // structs
                {
                    var lv = ilGen.DeclareLocal(type);
                    ilGen.Emit(OpCodes.Ldarg_0);
                    ilGen.Emit(OpCodes.Unbox_Any, type);
                    ilGen.Emit(OpCodes.Stloc_0);
                    ilGen.Emit(OpCodes.Ldloca_S, lv);
                    ilGen.Emit(OpCodes.Ldarg_1);
                    if (prop.PropertyType.IsClass)
                        ilGen.Emit(OpCodes.Castclass, prop.PropertyType);
                    else
                        ilGen.Emit(OpCodes.Unbox_Any, prop.PropertyType);
                    ilGen.EmitCall(OpCodes.Call, setMethod, null);
                    ilGen.Emit(OpCodes.Ldloc_0);
                    ilGen.Emit(OpCodes.Box, type);
                }
                else
                {
                    if (!setMethod.IsStatic)
                    {
                        ilGen.Emit(OpCodes.Ldarg_0);
                        ilGen.Emit(OpCodes.Castclass, prop.DeclaringType);
                        ilGen.Emit(OpCodes.Ldarg_1);
                        if (prop.PropertyType.IsClass)
                            ilGen.Emit(OpCodes.Castclass, prop.PropertyType);
                        else
                            ilGen.Emit(OpCodes.Unbox_Any, prop.PropertyType);
                        ilGen.EmitCall(OpCodes.Callvirt, setMethod, null);
                        ilGen.Emit(OpCodes.Ldarg_0);
                    }
                    else // unknown why is here at all....
                    {
                        ilGen.Emit(OpCodes.Ldarg_0);
                        ilGen.Emit(OpCodes.Ldarg_1);
                        if (prop.PropertyType.IsClass)
                            ilGen.Emit(OpCodes.Castclass, prop.PropertyType);
                        else
                            ilGen.Emit(OpCodes.Unbox_Any, prop.PropertyType);
                        ilGen.Emit(OpCodes.Call, setMethod);
                    }
                }

                ilGen.Emit(OpCodes.Ret);

                return (GenericSetter)dynSetter.CreateDelegate(typeof(GenericSetter));
            }
            catch (Exception exc)
            {
                throw new Exception(string.Format("Failed to compile generic setter method for property '{0}' on type '{1}' from assembly '{2}'.",
                    prop.Name, type.FullName, type.AssemblyQualifiedName), exc);
            }
        }

        #endregion

    }
}
