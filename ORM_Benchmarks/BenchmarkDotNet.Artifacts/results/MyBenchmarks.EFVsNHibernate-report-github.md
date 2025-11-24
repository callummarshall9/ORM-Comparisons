```

BenchmarkDotNet v0.15.6, Linux Ubuntu 24.04.3 LTS (Noble Numbat)
13th Gen Intel Core i9-13900K 0.80GHz, 1 CPU, 32 logical and 24 physical cores
.NET SDK 10.0.100
  [Host]     : .NET 10.0.0 (10.0.0, 10.0.25.52411), X64 RyuJIT x86-64-v3
  DefaultJob : .NET 10.0.0 (10.0.0, 10.0.25.52411), X64 RyuJIT x86-64-v3


```
| Method                  | Mean       | Error    | StdDev   | Gen0   | Allocated |
|------------------------ |-----------:|---------:|---------:|-------:|----------:|
| EF_RetrieveAll          |   262.9 μs |  5.07 μs |  6.59 μs | 2.9297 |  64.16 KB |
| NHibernate_RetrieveAll  |   305.7 μs |  5.61 μs | 13.77 μs | 5.8594 |  110.5 KB |
| AdoNet_RetrieveAll      |   259.1 μs |  4.90 μs | 10.96 μs | 1.9531 |  42.73 KB |
| EF_RetrieveById         |   106.8 μs |  3.55 μs | 10.08 μs | 0.2441 |   4.78 KB |
| NHibernate_RetrieveById |   519.9 μs | 15.67 μs | 46.20 μs | 0.9766 |  28.47 KB |
| AdoNet_RetrieveById     |   110.7 μs |  2.17 μs |  3.74 μs | 0.2441 |   5.95 KB |
| EF_AddRecord            | 1,308.3 μs | 26.14 μs | 76.25 μs |      - |  14.76 KB |
| NHibernate_AddRecord    | 1,387.6 μs | 27.49 μs | 43.60 μs |      - |  19.87 KB |
| AdoNet_AddRecord        | 1,152.3 μs | 22.61 μs | 30.18 μs |      - |   9.48 KB |
