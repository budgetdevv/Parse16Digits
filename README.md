# Parse16Digits
Micro-optimization approaches to parsing 16 digits of ulong string!

# Notes

- Atoi turns *= 10 into LEA + ADD

# Results

|     Method |      Mean |     Error |    StdDev | Code Size |
|----------- |----------:|----------:|----------:|----------:|
|   TrumpMcD |  1.802 ns | 0.0100 ns | 0.0093 ns |     125 B |
|      Atoi2 |  4.926 ns | 0.0024 ns | 0.0020 ns |     365 B |
|       Atoi | 10.332 ns | 0.0256 ns | 0.0239 ns |      68 B |
| UlongParse | 21.211 ns | 0.0551 ns | 0.0516 ns |   1,272 B |


# Hardware Info

BenchmarkDotNet=v0.13.1, OS=Windows 10.0.22000
Intel Core i7-8700K CPU 3.70GHz (Coffee Lake), 1 CPU, 12 logical and 6 physical cores
.NET SDK=7.0.100-preview.7.22377.5
  [Host]     : .NET 7.0.0 (7.0.22.37506), X64 RyuJIT
  DefaultJob : .NET 7.0.0 (7.0.22.37506), X64 RyuJIT

