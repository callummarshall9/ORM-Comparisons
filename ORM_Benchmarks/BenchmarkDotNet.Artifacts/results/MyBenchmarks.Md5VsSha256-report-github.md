```

BenchmarkDotNet v0.15.6, Linux Ubuntu 24.04.3 LTS (Noble Numbat)
13th Gen Intel Core i9-13900K 0.80GHz, 1 CPU, 32 logical and 24 physical cores
.NET SDK 9.0.306
  [Host]     : .NET 9.0.10 (9.0.10, 9.0.1025.47515), X64 RyuJIT x86-64-v3
  DefaultJob : .NET 9.0.10 (9.0.10, 9.0.1025.47515), X64 RyuJIT x86-64-v3


```
| Method | Mean     | Error     | StdDev    |
|------- |---------:|----------:|----------:|
| Sha256 | 3.969 μs | 0.0206 μs | 0.0193 μs |
| Md5    | 8.578 μs | 0.0471 μs | 0.0417 μs |
