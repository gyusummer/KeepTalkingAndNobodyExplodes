using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public static class RandomUtil
{
    private static Random random = new Random();

    // 배열에서 랜덤하게 a개 뽑고 인덱스 기준 정렬
    public static T[] PickRandomCombinationSortedByIndex<T>(T[] array, int a)
    {
        if (a > array.Length)
            throw new ArgumentException("뽑으려는 개수가 배열보다 많음");

        int[] indices = new int[array.Length];
        for (int i = 0; i < array.Length; i++)
            indices[i] = i;

        // Fisher–Yates Shuffle (인덱스 셔플)
        for (int i = indices.Length - 1; i > 0; i--)
        {
            int j = random.Next(i + 1);
            (indices[i], indices[j]) = (indices[j], indices[i]);
        }

        // 앞 a개만 정렬
        Array.Sort(indices, 0, a);

        // 결과 배열 생성
        T[] result = new T[a];
        for (int i = 0; i < a; i++)
            result[i] = array[indices[i]];

        return result;
    }
}