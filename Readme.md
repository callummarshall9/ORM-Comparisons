| Method                             | Mean        | Error      | StdDev     | Median      | Gen0     | Gen1    | Allocated  |
|----------------------------------- |------------:|-----------:|-----------:|------------:|---------:|--------:|-----------:|
| EF_RetrieveAll                     |   146.70 us |   2.917 us |   5.550 us |   146.57 us |   3.9063 |  0.4883 |   75.84 KB |
| NHibernate_RetrieveAll             |   168.53 us |   3.350 us |   6.455 us |   168.39 us |   5.8594 |  0.4883 |   110.5 KB |
| AdoNet_RetrieveAll                 |   135.34 us |   1.774 us |   1.572 us |   135.74 us |   2.1973 |       - |   42.73 KB |
| EF_RetrieveById                    |    55.65 us |   2.030 us |   5.727 us |    53.42 us |   0.2441 |       - |     4.9 KB |
| NHibernate_RetrieveById            |    74.22 us |   3.622 us |  10.157 us |    70.72 us |   1.7090 |       - |    32.7 KB |
| AdoNet_RetrieveById                |    56.34 us |   1.124 us |   1.998 us |    55.88 us |   0.2441 |       - |    5.95 KB |
| EF_RetrieveAllWithIncludes         | 1,752.99 us |  34.026 us |  41.787 us | 1,755.38 us |  66.4063 | 27.3438 | 1269.93 KB |
| NHibernate_RetrieveAllWithIncludes | 6,010.57 us | 109.327 us | 102.264 us | 6,035.00 us | 335.9375 | 62.5000 | 6276.97 KB |
| EF_AddRecord                       |   984.99 us |  19.205 us |  24.288 us |   988.88 us |        - |       - |   18.27 KB |
| NHibernate_AddRecord               | 1,213.09 us |  24.060 us |  51.791 us | 1,195.70 us |        - |       - |   22.67 KB |
| AdoNet_AddRecord                   |   933.74 us |  18.624 us |  44.623 us |   928.68 us |        - |       - |    9.48 KB |