| Method                  | Mean       | Error    | StdDev   | Gen0   | Allocated |
|------------------------ |-----------:|---------:|---------:|-------:|----------:|
| EF_RetrieveAll          |   262.9 us |  5.07 us |  6.59 us | 2.9297 |  64.16 KB |
| NHibernate_RetrieveAll  |   305.7 us |  5.61 us | 13.77 us | 5.8594 |  110.5 KB |
| AdoNet_RetrieveAll      |   259.1 us |  4.90 us | 10.96 us | 1.9531 |  42.73 KB |
| EF_AddRecord            | 1,308.3 us | 26.14 us | 76.25 us |      - |  14.76 KB |
| NHibernate_AddRecord    | 1,387.6 us | 27.49 us | 43.60 us |      - |  19.87 KB |
| AdoNet_AddRecord        | 1,152.3 us | 22.61 us | 30.18 us |      - |   9.48 KB |

| Method                  | Mean     | Error    | StdDev   | Median   | Gen0   | Gen1   | Allocated |
|------------------------ |---------:|---------:|---------:|---------:|-------:|-------:|----------:|
| EF_RetrieveById         | 58.36 us | 2.154 us | 6.248 us | 57.47 us | 0.2441 |      - |   4.78 KB |
| NHibernate_RetrieveById | 65.56 us | 2.528 us | 7.295 us | 63.18 us | 1.7090 | 0.1221 |  31.82 KB |
| AdoNet_RetrieveById     | 55.62 us | 1.089 us | 1.663 us | 55.42 us | 0.2441 |      - |   5.95 KB |