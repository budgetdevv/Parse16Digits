using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Runtime.Intrinsics;
using System.Runtime.Intrinsics.X86;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Order;

[Orderer(SummaryOrderPolicy.FastestToSlowest)]
[DisassemblyDiagnoser(exportDiff: true, exportCombinedDisassemblyReport: true)]
public class Bench
{
    [ModuleInitializer]
    public static void RunCctor()
    {
        RuntimeHelpers.RunClassConstructor(typeof(Bench).TypeHandle);
    }

    static Bench()
    {
        var Instance = Unsafe.As<Bench>(RuntimeHelpers.GetUninitializedObject(typeof(Bench)));

        var Result = Instance.TrumpMcD();
        
        if (Result != 1234567812345678)
        {
            throw new Exception(Result.ToString());
        }
    }

    public const string ID = "1234567812345678";

    [Benchmark]
    public ulong Atoi2()
    {
        var Span = ID.AsSpan();

        ref var Current = ref MemoryMarshal.GetReference(Span);

        var E0 = (ulong) (Current - '0');
        
        var E1 = (ulong) (Unsafe.Add(ref Current, 1) - '0') * 10;
        
        var E2 = (ulong) (Unsafe.Add(ref Current, 2) - '0') * 100;
        
        var E3 = (ulong) (Unsafe.Add(ref Current, 3) - '0') * 1000;
        
        var E4 = (ulong) (Unsafe.Add(ref Current, 4) - '0') * 10_000;
        
        var E5 = (ulong) (Unsafe.Add(ref Current, 5) - '0') * 100_000;
        
        var E6 = (ulong) (Unsafe.Add(ref Current, 6) - '0') * 1_000_000;
        
        var E7 = (ulong) (Unsafe.Add(ref Current, 7) - '0') * 10_000_000;
        
        var E8 = (ulong) (Unsafe.Add(ref Current, 8) - '0') * 100_000_000;
        
        var E9 = (ulong) (Unsafe.Add(ref Current, 9) - '0') * 1_000_000_000;
        
        var E10 = (ulong) (Unsafe.Add(ref Current, 10) - '0') * 10_000_000_000;
        
        var E11 = (ulong) (Unsafe.Add(ref Current, 11) - '0') * 100_000_000_000;
        
        var E12 = (ulong) (Unsafe.Add(ref Current, 12) - '0') * 1_000_000_000_000;
        
        var E13 = (ulong) (Unsafe.Add(ref Current, 13) - '0') * 10_000_000_000_000;
        
        var E14 = (ulong) (Unsafe.Add(ref Current, 14) - '0') * 100_000_000_000_000;
        
        var E15 = (ulong) (Unsafe.Add(ref Current, 15) - '0') * 1_000_000_000_000_000;

        return E0 + E1 + E2 + E3 + E4 + E5 + E6 + E7 + E8 + E9 + E10 + E11 + E12 + E13 + E14 + E15;
    }
    
    [Benchmark]
    public ulong UlongParse()
    {
        return ulong.Parse(ID);
    }

    [Benchmark]
    public ulong Atoi()
    {
        var Span = ID.AsSpan();

        ref var Current = ref MemoryMarshal.GetReference(Span);
        
        ref var LastOffsetbyOne = ref Unsafe.Add(ref Current, Span.Length);

        ulong Accumulator = 0;
        
        for (;; Current = ref Unsafe.Add(ref Current, 1), Accumulator *= 10)
        {
            Accumulator += (ulong) (Current - '0');

            if (Unsafe.AreSame(ref Current, ref LastOffsetbyOne))
            {
                break;
            }
        }

        return Accumulator;
    }
    
    [Benchmark]
    public ulong TrumpMcD()
    {
        var Span = ID.AsSpan();

        ref var First = ref MemoryMarshal.GetReference(Span);

        var Vec = Vector256.LoadUnsafe(ref Unsafe.As<char, short>(ref First));

        var OffsetVec = Vector256.Create((short) '0');

        //1 2 3 4 5 6 7 8 | 1 2 3 4 5 6 7 8
        Vec = Avx2.Subtract(Vec, OffsetVec);

        //We face 2 fundamental problems here -
        //1) int.MaxValue is only 10 digits, we need 16 here
        //2) Each V128 register support 8 ushorts, but ushort.MaxValue is 5 digits

        var MulVec = Vector256.Create((short) 1000, 1000, 1000, 1000, 1000, 100, 10, 1, 1000, 1000, 1000, 1000, 1000, 100, 10, 1);
        
        //Multiply and store lower 16 bits ( Which would be the whole value anyway )
        Vec = Avx2.MultiplyLow(Vec, MulVec);
        
        var UpperVec = Vec.GetUpper();

        var LowerVec = Vec.GetLower();

        //Multiply the first 4 since previous multiplication short changed them
        var MulVec2 = Vector128.Create((short) 10_000, 1000, 100, 10, 1, 1, 1, 1);

        //12 34 56 78
        var UpperVec2 = Avx2.MultiplyAddAdjacent(UpperVec, MulVec2);
        
        var LowerVec2 = Avx2.MultiplyAddAdjacent(LowerVec, MulVec2);

        //Dammit give us macros already >:(
        const byte RightToLeftShuffleMask = (1 << 6) + (0 << 4) + (3 << 2) + 2; //1 0 3 2

        //56 78 12 34
        var UpperVecShuffle = Avx2.Shuffle(UpperVec2, RightToLeftShuffleMask);

        var LowerVecShuffle = Avx2.Shuffle(LowerVec2, RightToLeftShuffleMask);

        //( The goal here to add them in such a way that the first element becomes the sum of all 4 elements )
        //12 34 56 78 + 56 78 12 34 = 1256 3478 1256 3478
        UpperVec2 = Avx2.Add(UpperVec2, UpperVecShuffle);
        
        LowerVec2 = Avx2.Add(LowerVec2, LowerVecShuffle);

        //Just do it again, once more

        const byte RightToLeftShuffleMask2 = (3 << 6) + (2 << 4) + (0 << 2) + 1; //3 2 0 1

        UpperVecShuffle = Avx2.Shuffle(UpperVec2, RightToLeftShuffleMask2);
        
        LowerVecShuffle = Avx2.Shuffle(LowerVec2, RightToLeftShuffleMask2);

        UpperVec2 = Avx2.Add(UpperVec2, UpperVecShuffle);

        LowerVec2 = Avx2.Add(LowerVec2, LowerVecShuffle);

        var UpperVal = (ulong) UpperVec2[0];

        var LowerVal = (ulong) LowerVec2[0] * 1_0000_0000;

        return UpperVal + LowerVal;
    }
}