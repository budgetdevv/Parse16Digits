# Parse16Digits
Micro-optimization approaches to parsing 16 digits of ulong string!

# Notes

- Atoi turns *= 10 into LEA + ADD

# Results

// * Summary *

BenchmarkDotNet=v0.13.1, OS=Windows 10.0.22000
Intel Core i7-8700K CPU 3.70GHz (Coffee Lake), 1 CPU, 12 logical and 6 physical cores
.NET SDK=7.0.100-preview.7.22377.5
  [Host]     : .NET 7.0.0 (7.0.22.37506), X64 RyuJIT
  DefaultJob : .NET 7.0.0 (7.0.22.37506), X64 RyuJIT


|     Method |      Mean |     Error |    StdDev | Code Size |
|----------- |----------:|----------:|----------:|----------:|
|   TrumpMcD |  1.823 ns | 0.0094 ns | 0.0088 ns |     125 B |
|      Atoi2 |  6.439 ns | 0.0369 ns | 0.0327 ns |     365 B |
|       Atoi | 11.199 ns | 0.0588 ns | 0.0550 ns |      68 B |
| UlongParse | 21.212 ns | 0.0515 ns | 0.0457 ns |   1,272 B |

// * Hints *
Outliers
  Bench.Atoi2: Default      -> 1 outlier  was  removed (8.08 ns)
  Bench.UlongParse: Default -> 1 outlier  was  removed (22.82 ns)

// * Legends *
  Mean      : Arithmetic mean of all measurements
  Error     : Half of 99.9% confidence interval
  StdDev    : Standard deviation of all measurements
  Code Size : Native code size of the disassembled method(s)
  1 ns      : 1 Nanosecond (0.000000001 sec)

// * Diagnostic Output - DisassemblyDiagnoser *
Disassembled benchmarks got exported to ".\BenchmarkDotNet.Artifacts\results\*asm.md"

// ***** BenchmarkRunner: End *****
// ** Remained 0 benchmark(s) to run **
Run time: 00:01:43 (103.29 sec), executed benchmarks: 4

Global total time: 00:01:46 (106.61 sec), executed benchmarks: 4
// * Artifacts cleanup *
