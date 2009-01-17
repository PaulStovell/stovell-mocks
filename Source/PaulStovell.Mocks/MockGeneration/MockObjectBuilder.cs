using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using System.Text;
using System.Threading;
using PaulStovell.Mocks.Interfaces;

namespace PaulStovell.Mocks.MockGeneration
{
    /// <summary>
    /// Controls the automatic code generation of a mock object.
    /// </summary>
    internal sealed class MockObjectBuilder
    {
        private TypeBuilder _typeBuilder;
        private Type _interfaceToImplement;
        private Type _generatedType;
        private FieldBuilder __recorder0;

        /// <summary>
        /// Initializes a new instance of the <see cref="MockObjectBuilder"/> class.
        /// </summary>
        /// <param name="interfaceToImplement">The interface to implement.</param>
        public MockObjectBuilder(Type interfaceToImplement)
        {
            _interfaceToImplement = interfaceToImplement;
        }

        /// <summary>
        /// Generates the type.
        /// </summary>
        /// <returns></returns>
        public Type GenerateType()
        {
            if (_generatedType == null)
            {
                // Setup the assembly and type builder
                AssemblyName assemblyName = new AssemblyName("Mock.Generated-" + _interfaceToImplement.Name);
                AssemblyBuilder assemblyBuilder = Thread.GetDomain().DefineDynamicAssembly(assemblyName, AssemblyBuilderAccess.Run);
                ModuleBuilder moduleBuilder = assemblyBuilder.DefineDynamicModule(assemblyName.Name);
                _typeBuilder = moduleBuilder.DefineType("Mock.Generated.Mock" + _interfaceToImplement.Name, TypeAttributes.Public);
                _typeBuilder.AddInterfaceImplementation(_interfaceToImplement);

                // Generate the body of the type
                this.CreateFields();
                this.CreateConstructor();
                foreach (MethodInfo method in _interfaceToImplement.GetMethods(BindingFlags.Public | BindingFlags.Instance).Where(m => !m.Name.StartsWith("get_")))
                {
                    this.ImplementMethod(method);
                }
                foreach (PropertyInfo property in _interfaceToImplement.GetProperties(BindingFlags.Public | BindingFlags.Instance))
                {
                    this.ImplementProperty(property);
                }

                // Store the generated type for use again
                _generatedType = _typeBuilder.CreateType();
            }
            return _generatedType;
        }

        /// <summary>
        /// Creates the fields.
        /// </summary>
        private void CreateFields()
        {
            __recorder0 = _typeBuilder.DefineField("__recorder0", typeof(IMockRecorder), FieldAttributes.Private);
        }

        /// <summary>
        /// Creates the constructor. The generated constructor will take one parameter - an IMockRecorder that will be used to 
        /// record the mock object behavior during the test execution.
        /// </summary>
        private void CreateConstructor()
        {
            ConstructorBuilder constructor = _typeBuilder.DefineConstructor(MethodAttributes.Public | MethodAttributes.RTSpecialName, CallingConventions.Standard, new Type[] { typeof(IMockRecorder) });
            ILGenerator constructorILGenerator = constructor.GetILGenerator();
            constructorILGenerator.Emit(OpCodes.Ldarg_0);
            ConstructorInfo originalConstructor = typeof(Object).GetConstructor(BindingFlags.Instance | BindingFlags.Public, null, new Type[0], null);
            constructorILGenerator.Emit(OpCodes.Call, originalConstructor);
            constructorILGenerator.Emit(OpCodes.Ldarg_0);
            constructorILGenerator.Emit(OpCodes.Ldarg_1);
            constructorILGenerator.Emit(OpCodes.Stfld, __recorder0);
            constructorILGenerator.Emit(OpCodes.Ret);
        }

        /// <summary>
        /// Implements a method on the mocked object. The method will simply call down to the 
        /// </summary>
        /// <param name="methodToImplement">The method to implement.</param>
        private void ImplementMethod(MethodInfo methodToImplement)
        {
            IEnumerable<Type> parameterTypes = methodToImplement.GetParameters().Select(p => p.ParameterType);
            MethodBuilder methodBuilder = _typeBuilder.DefineMethod(methodToImplement.Name, MethodAttributes.Public | MethodAttributes.Virtual, methodToImplement.ReturnType, parameterTypes.ToArray());
            methodBuilder.CreateMethodBody(null, 0);
            ILGenerator methodILGenerator = methodBuilder.GetILGenerator();
            LocalBuilder resultLocalBuilder = methodILGenerator.DeclareLocal(methodToImplement.ReturnType);
            LocalBuilder parametersLocalBuilder = methodILGenerator.DeclareLocal(typeof(object[]));
            methodILGenerator.Emit(OpCodes.Ldarg_0);
            methodILGenerator.Emit(OpCodes.Ldfld, __recorder0);
            methodILGenerator.Emit(OpCodes.Call, GetMethodBaseGetCurrentMethod());
            methodILGenerator.Emit(OpCodes.Ldc_I4_1);
            methodILGenerator.Emit(OpCodes.Newarr, typeof(object));
            methodILGenerator.Emit(OpCodes.Stloc_1);
            methodILGenerator.Emit(OpCodes.Ldloc_1);
            for (int parameterIndex = 0; parameterIndex < methodToImplement.GetParameters().Length; parameterIndex++)
            {
                methodILGenerator.Emit(OpCodes.Ldc_I4, parameterIndex);
                methodILGenerator.Emit(OpCodes.Ldarg, parameterIndex + 1);
                methodILGenerator.Emit(OpCodes.Stelem_Ref);
            }
            methodILGenerator.Emit(OpCodes.Ldloc_1);
            methodILGenerator.Emit(OpCodes.Callvirt, GetIMockRecorderMethodCallFor(methodToImplement.ReturnType));
            methodILGenerator.Emit(OpCodes.Stloc_0);
            methodILGenerator.Emit(OpCodes.Ldloc_0);
            methodILGenerator.Emit(OpCodes.Ret);
            _typeBuilder.DefineMethodOverride(methodBuilder, methodToImplement);
        }

        /// <summary>
        /// Implements a given get-only property on the current mock object.
        /// </summary>
        /// <param name="propertyToImplement">The property to implement.</param>
        private void ImplementProperty(PropertyInfo propertyToImplement)
        {
            MethodInfo getterMethodInfo = propertyToImplement.GetGetMethod();
            MethodBuilder methodBuilder = _typeBuilder.DefineMethod(getterMethodInfo.Name, MethodAttributes.Public | MethodAttributes.HideBySig | MethodAttributes.SpecialName | MethodAttributes.Virtual | MethodAttributes.NewSlot | MethodAttributes.Final, getterMethodInfo.ReturnType, null);
            ILGenerator methodILGenerator = methodBuilder.GetILGenerator();
            LocalBuilder resultLocalBuilder = methodILGenerator.DeclareLocal(propertyToImplement.PropertyType);
            methodILGenerator.Emit(OpCodes.Ldarg_0);
            methodILGenerator.Emit(OpCodes.Ldfld, __recorder0);
            methodILGenerator.Emit(OpCodes.Call, GetMethodBaseGetCurrentMethod());
            methodILGenerator.Emit(OpCodes.Ldc_I4_0);
            methodILGenerator.Emit(OpCodes.Newarr, typeof(object));                 // Create the array that will hold all the parameters to pass to the call method
            methodILGenerator.Emit(OpCodes.Callvirt, GetIMockRecorderMethodCallFor(propertyToImplement.PropertyType));  
            methodILGenerator.Emit(OpCodes.Stloc_0);
            methodILGenerator.Emit(OpCodes.Ldloc_0);
            methodILGenerator.Emit(OpCodes.Ret);
            PropertyBuilder propertyBuilder = _typeBuilder.DefineProperty(propertyToImplement.Name, PropertyAttributes.HasDefault, propertyToImplement.PropertyType, Type.EmptyTypes);
            propertyBuilder.SetGetMethod(methodBuilder);
        }

        /// <summary>
        /// Gets the MethodBase.GetCurrentMethod method info.
        /// </summary>
        /// <returns></returns>
        private static MethodInfo GetMethodBaseGetCurrentMethod() 
        {
            return typeof(MethodBase).GetMethod("GetCurrentMethod", BindingFlags.Public | BindingFlags.Static);
        }

        /// <summary>
        /// Gets the IMockRecorder.MethodCall MethodInfo[TReturn] for a given type of TReturn.
        /// </summary>
        /// <param name="type">The return type of the method or property being mocked.</param>
        /// <returns></returns>
        private static MethodInfo GetIMockRecorderMethodCallFor(Type type)
        {
            MethodInfo recordCallMethodInfo = typeof(IMockRecorder).GetMethod("MethodCall", BindingFlags.Public | BindingFlags.Instance);
            recordCallMethodInfo = recordCallMethodInfo.MakeGenericMethod(type);
            return recordCallMethodInfo;
        }
    }
}
