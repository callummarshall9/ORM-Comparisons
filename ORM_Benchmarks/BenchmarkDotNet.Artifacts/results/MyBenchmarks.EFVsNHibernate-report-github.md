```

BenchmarkDotNet v0.15.6, Linux Ubuntu 24.04.3 LTS (Noble Numbat)
13th Gen Intel Core i9-13900K 0.80GHz, 1 CPU, 32 logical and 24 physical cores
.NET SDK 10.0.100
  [Host]     : .NET 10.0.0 (10.0.0, 10.0.25.52411), X64 RyuJIT x86-64-v3
  DefaultJob : .NET 10.0.0 (10.0.0, 10.0.25.52411), X64 RyuJIT x86-64-v3


```
| Method                  | Mean     | Error    | StdDev   | Median   | Gen0   | Gen1   | Allocated |
|------------------------ |---------:|---------:|---------:|---------:|-------:|-------:|----------:|
| EF_RetrieveById         | 58.36 μs | 2.154 μs | 6.248 μs | 57.47 μs | 0.2441 |      - |   4.78 KB |
| NHibernate_RetrieveById | 65.56 μs | 2.528 μs | 7.295 μs | 63.18 μs | 1.7090 | 0.1221 |  31.82 KB |
| AdoNet_RetrieveById     | 55.62 μs | 1.089 μs | 1.663 μs | 55.42 μs | 0.2441 |      - |   5.95 KB |
