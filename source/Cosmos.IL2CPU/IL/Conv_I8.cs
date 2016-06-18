using System;
using XSharp.Compiler;
using static XSharp.Compiler.XSRegisters;
using CPUx86 = Cosmos.Assembler.x86;

namespace Cosmos.IL2CPU.X86.IL
{
	/// <summary>
	/// Convert top Stack element to Int64.
	/// </summary>
    [Cosmos.IL2CPU.OpCode( ILOpCode.Code.Conv_I8 )]
    public class Conv_I8 : ILOp
    {
        public Conv_I8( Cosmos.Assembler.Assembler aAsmblr )
            : base( aAsmblr )
        {
        }

        public override void Execute( MethodInfo aMethod, ILOpCode aOpCode )
        {
            var xSource = aOpCode.StackPopTypes[0];
            var xSourceSize = SizeOfType(xSource);
            var xSourceIsFloat = TypeIsFloat(xSource);
            switch( xSourceSize )
            {
                case 1:
                    throw new Exception("Cosmos.IL2CPU.x86->IL->Conv_I8.cs->The size 1 could not exist, because always is pushed Int32 or Int64!");
                case 2:
                    throw new Exception("Cosmos.IL2CPU.x86->IL->Conv_I8.cs->The size 2 could not exist, because always is pushed Int32 or Int64!");
                case 4:
					if (xSourceIsFloat)
					{
						XS.FPU.FloatLoad(ESP, destinationIsIndirect: true, size: RegisterSize.Int32);
						XS.Sub(OldToNewRegister(CPUx86.RegistersEnum.ESP), 4);
						XS.FPU.IntStoreWithTruncate(ESP, isIndirect: true, size: RegisterSize.Long64);
					}
					else
					{
						XS.Pop(OldToNewRegister(CPUx86.RegistersEnum.EAX));
						XS.SignExtendAX(RegisterSize.Int32);
						XS.Push(OldToNewRegister(CPUx86.RegistersEnum.EDX));
                        XS.Push(OldToNewRegister(CPUx86.RegistersEnum.EAX));
					}
                    break;
                case 8:
					if (xSourceIsFloat)
					{
						XS.FPU.FloatLoad(ESP, destinationIsIndirect: true, size: RegisterSize.Long64);
						XS.FPU.IntStoreWithTruncate(ESP, isIndirect: true, size: RegisterSize.Long64);
					}
                    break;
                default:
                    //EmitNotImplementedException(Assembler, GetServiceProvider(), "Conv_I8: SourceSize " + xSource + " not supported!", mCurLabel, mMethodInformation, mCurOffset, mNextLabel);
                    throw new NotImplementedException();
            }
        }
    }
}
