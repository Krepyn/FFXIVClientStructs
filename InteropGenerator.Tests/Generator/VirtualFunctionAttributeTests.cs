using Xunit;
using VerifyIG = InteropGenerator.Tests.Helpers.IncrementalGeneratorVerifier<InteropGenerator.Generator.InteropGenerator>;

namespace InteropGenerator.Tests.Generator;

public class VirtualFunctionAttributeTests {
    [Fact]
    public async Task GenerateVirtualFunction() {
        const string code = """
                            [global::System.Runtime.InteropServices.StructLayoutAttribute(global::System.Runtime.InteropServices.LayoutKind.Explicit)]
                            [GenerateInterop]
                            public unsafe partial struct TestStruct
                            {
                                [VirtualFunction(5)]
                                public partial int TestFunction(int argOne, void * argTwo);
                            }
                            """;

        const string result = """
                              // <auto-generated/>
                              unsafe partial struct TestStruct
                              {
                                  [global::System.Runtime.InteropServices.StructLayoutAttribute(global::System.Runtime.InteropServices.LayoutKind.Explicit)]
                                  public unsafe partial struct TestStructVirtualTable
                                  {
                                      [global::System.Runtime.InteropServices.FieldOffsetAttribute(40)] public delegate* unmanaged <TestStruct*, int, void*, int> TestFunction;
                                  }
                                  [global::System.Runtime.InteropServices.FieldOffsetAttribute(0)] public TestStructVirtualTable* VirtualTable;
                                  public static partial class Delegates
                                  {
                                      public delegate int TestFunction(int argOne, void* argTwo);
                                  }
                                  [global::System.Runtime.CompilerServices.MethodImplAttribute(global::System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
                                  public partial int TestFunction(int argOne, void* argTwo) => VirtualTable->TestFunction((TestStruct*)global::System.Runtime.CompilerServices.Unsafe.AsPointer(ref this), argOne, argTwo);
                              }
                              """;

        await VerifyIG.VerifyGeneratorAsync(
            code,
            ("TestStruct.InteropGenerator.g.cs", result));
    }

    [Fact]
    public async Task GenerateVirtualFunctionNoReturn() {
        const string code = """
                            [global::System.Runtime.InteropServices.StructLayoutAttribute(global::System.Runtime.InteropServices.LayoutKind.Explicit)]
                            [GenerateInterop]
                            public unsafe partial struct TestStruct
                            {
                                [VirtualFunction(5)]
                                public partial void TestFunction(int argOne, void * argTwo);
                            }
                            """;

        const string result = """
                              // <auto-generated/>
                              unsafe partial struct TestStruct
                              {
                                  [global::System.Runtime.InteropServices.StructLayoutAttribute(global::System.Runtime.InteropServices.LayoutKind.Explicit)]
                                  public unsafe partial struct TestStructVirtualTable
                                  {
                                      [global::System.Runtime.InteropServices.FieldOffsetAttribute(40)] public delegate* unmanaged <TestStruct*, int, void*, void> TestFunction;
                                  }
                                  [global::System.Runtime.InteropServices.FieldOffsetAttribute(0)] public TestStructVirtualTable* VirtualTable;
                                  public static partial class Delegates
                                  {
                                      public delegate void TestFunction(int argOne, void* argTwo);
                                  }
                                  [global::System.Runtime.CompilerServices.MethodImplAttribute(global::System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
                                  public partial void TestFunction(int argOne, void* argTwo) => VirtualTable->TestFunction((TestStruct*)global::System.Runtime.CompilerServices.Unsafe.AsPointer(ref this), argOne, argTwo);
                              }
                              """;

        await VerifyIG.VerifyGeneratorAsync(
            code,
            ("TestStruct.InteropGenerator.g.cs", result));
    }

    [Fact]
    public async Task GenerateVirtualFunctionInNamespace() {
        const string code = """
                            namespace TestNamespace.InnerNamespace;

                            [global::System.Runtime.InteropServices.StructLayoutAttribute(global::System.Runtime.InteropServices.LayoutKind.Explicit)]
                            [GenerateInterop]
                            public unsafe partial struct TestStruct
                            {
                                [VirtualFunction(5)]
                                public partial int TestFunction(int argOne, void * argTwo);
                            }
                            """;

        const string result = """
                              // <auto-generated/>
                              namespace TestNamespace.InnerNamespace;
                              
                              unsafe partial struct TestStruct
                              {
                                  [global::System.Runtime.InteropServices.StructLayoutAttribute(global::System.Runtime.InteropServices.LayoutKind.Explicit)]
                                  public unsafe partial struct TestStructVirtualTable
                                  {
                                      [global::System.Runtime.InteropServices.FieldOffsetAttribute(40)] public delegate* unmanaged <TestStruct*, int, void*, int> TestFunction;
                                  }
                                  [global::System.Runtime.InteropServices.FieldOffsetAttribute(0)] public TestStructVirtualTable* VirtualTable;
                                  public static partial class Delegates
                                  {
                                      public delegate int TestFunction(int argOne, void* argTwo);
                                  }
                                  [global::System.Runtime.CompilerServices.MethodImplAttribute(global::System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
                                  public partial int TestFunction(int argOne, void* argTwo) => VirtualTable->TestFunction((TestStruct*)global::System.Runtime.CompilerServices.Unsafe.AsPointer(ref this), argOne, argTwo);
                              }
                              """;

        await VerifyIG.VerifyGeneratorAsync(
            code,
            ("TestNamespace.InnerNamespace.TestStruct.InteropGenerator.g.cs", result));
    }

    [Fact]
    public async Task GenerateVirtualFunctionInnerStruct() {
        const string code = """
                            public unsafe partial struct TestStruct
                            {
                                [global::System.Runtime.InteropServices.StructLayoutAttribute(global::System.Runtime.InteropServices.LayoutKind.Explicit)]
                                [GenerateInterop]
                                public unsafe partial struct InnerStruct
                                {
                                    [VirtualFunction(5)]
                                    public partial int TestFunction(int argOne, void * argTwo);
                                }
                            }
                            """;

        const string result = """
                              // <auto-generated/>
                              unsafe partial struct TestStruct
                              {
                                  unsafe partial struct InnerStruct
                                  {
                                      [global::System.Runtime.InteropServices.StructLayoutAttribute(global::System.Runtime.InteropServices.LayoutKind.Explicit)]
                                      public unsafe partial struct InnerStructVirtualTable
                                      {
                                          [global::System.Runtime.InteropServices.FieldOffsetAttribute(40)] public delegate* unmanaged <InnerStruct*, int, void*, int> TestFunction;
                                      }
                                      [global::System.Runtime.InteropServices.FieldOffsetAttribute(0)] public InnerStructVirtualTable* VirtualTable;
                                      public static partial class Delegates
                                      {
                                          public delegate int TestFunction(int argOne, void* argTwo);
                                      }
                                      [global::System.Runtime.CompilerServices.MethodImplAttribute(global::System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
                                      public partial int TestFunction(int argOne, void* argTwo) => VirtualTable->TestFunction((InnerStruct*)global::System.Runtime.CompilerServices.Unsafe.AsPointer(ref this), argOne, argTwo);
                                  }
                              }
                              """;

        await VerifyIG.VerifyGeneratorAsync(
            code,
            ("TestStruct.InnerStruct.InteropGenerator.g.cs", result));
    }

    [Fact]
    public async Task GenerateVirtualFunctionParamModifier() {
        const string code = """
                            [global::System.Runtime.InteropServices.StructLayoutAttribute(global::System.Runtime.InteropServices.LayoutKind.Explicit)]
                            [GenerateInterop]
                            public unsafe partial struct TestStruct
                            {
                                [VirtualFunction(5)]
                                public partial int TestFunction(out int argOne, void * argTwo);
                            }
                            """;

        const string result = """
                              // <auto-generated/>
                              unsafe partial struct TestStruct
                              {
                                  [global::System.Runtime.InteropServices.StructLayoutAttribute(global::System.Runtime.InteropServices.LayoutKind.Explicit)]
                                  public unsafe partial struct TestStructVirtualTable
                                  {
                                      [global::System.Runtime.InteropServices.FieldOffsetAttribute(40)] public delegate* unmanaged <TestStruct*, out int, void*, int> TestFunction;
                                  }
                                  [global::System.Runtime.InteropServices.FieldOffsetAttribute(0)] public TestStructVirtualTable* VirtualTable;
                                  public static partial class Delegates
                                  {
                                      public delegate int TestFunction(out int argOne, void* argTwo);
                                  }
                                  [global::System.Runtime.CompilerServices.MethodImplAttribute(global::System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
                                  public partial int TestFunction(out int argOne, void* argTwo) => VirtualTable->TestFunction((TestStruct*)global::System.Runtime.CompilerServices.Unsafe.AsPointer(ref this), out argOne, argTwo);
                              }
                              """;

        await VerifyIG.VerifyGeneratorAsync(
            code,
            ("TestStruct.InteropGenerator.g.cs", result));
    }

    [Fact]
    public async Task GenerateVirtualFunctionMultiple() {
        const string code = """
                            [global::System.Runtime.InteropServices.StructLayoutAttribute(global::System.Runtime.InteropServices.LayoutKind.Explicit)]
                            [GenerateInterop]
                            public unsafe partial struct TestStruct
                            {
                                [VirtualFunction(5)]
                                public partial int TestFunction(int argOne, void * argTwo);
                                
                                [VirtualFunction(17)]
                                public partial void TestFunction2();
                            }
                            """;

        const string result = """
                              // <auto-generated/>
                              unsafe partial struct TestStruct
                              {
                                  [global::System.Runtime.InteropServices.StructLayoutAttribute(global::System.Runtime.InteropServices.LayoutKind.Explicit)]
                                  public unsafe partial struct TestStructVirtualTable
                                  {
                                      [global::System.Runtime.InteropServices.FieldOffsetAttribute(40)] public delegate* unmanaged <TestStruct*, int, void*, int> TestFunction;
                                      [global::System.Runtime.InteropServices.FieldOffsetAttribute(136)] public delegate* unmanaged <TestStruct*, void> TestFunction2;
                                  }
                                  [global::System.Runtime.InteropServices.FieldOffsetAttribute(0)] public TestStructVirtualTable* VirtualTable;
                                  public static partial class Delegates
                                  {
                                      public delegate int TestFunction(int argOne, void* argTwo);
                                      public delegate void TestFunction2();
                                  }
                                  [global::System.Runtime.CompilerServices.MethodImplAttribute(global::System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
                                  public partial int TestFunction(int argOne, void* argTwo) => VirtualTable->TestFunction((TestStruct*)global::System.Runtime.CompilerServices.Unsafe.AsPointer(ref this), argOne, argTwo);
                                  [global::System.Runtime.CompilerServices.MethodImplAttribute(global::System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
                                  public partial void TestFunction2() => VirtualTable->TestFunction2((TestStruct*)global::System.Runtime.CompilerServices.Unsafe.AsPointer(ref this));
                              }
                              """;

        await VerifyIG.VerifyGeneratorAsync(
            code,
            ("TestStruct.InteropGenerator.g.cs", result));
    }
}
