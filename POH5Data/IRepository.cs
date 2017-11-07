using System.Collections.Generic;

namespace POH5Data
{
    public interface IRepository<T>
    {
        bool Lisaa(T item);
        bool Poista(int index);
        bool Muuta(T item);

        T Hae(int index);

        List<T> HaeKaikki();
    }
}
