| Method                  | Mean       | Error    | StdDev   | Gen0   | Allocated |
|------------------------ |-----------:|---------:|---------:|-------:|----------:|
| EF_RetrieveAll          |   262.9 us |  5.07 us |  6.59 us | 2.9297 |  64.16 KB |
| NHibernate_RetrieveAll  |   305.7 us |  5.61 us | 13.77 us | 5.8594 |  110.5 KB |
| AdoNet_RetrieveAll      |   259.1 us |  4.90 us | 10.96 us | 1.9531 |  42.73 KB |
| EF_RetrieveById         |   106.8 us |  3.55 us | 10.08 us | 0.2441 |   4.78 KB |
| NHibernate_RetrieveById |   519.9 us | 15.67 us | 46.20 us | 0.9766 |  28.47 KB |
| AdoNet_RetrieveById     |   110.7 us |  2.17 us |  3.74 us | 0.2441 |   5.95 KB |
| EF_AddRecord            | 1,308.3 us | 26.14 us | 76.25 us |      - |  14.76 KB |
| NHibernate_AddRecord    | 1,387.6 us | 27.49 us | 43.60 us |      - |  19.87 KB |
| AdoNet_AddRecord        | 1,152.3 us | 22.61 us | 30.18 us |      - |   9.48 KB |