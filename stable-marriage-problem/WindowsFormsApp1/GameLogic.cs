using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowsFormsApp1
{
    public static class GameLogic
    {
		public static int lang;
        public static int mode;
		public static int seed;
		public static string difficulty;
		public static int menCount;
        public static int womenCount;
        public static List<List<int>> m_pref;
        public static List<List<int>> w_pref;
        public static List<int> pairs;
        public static void Randomize(List<List<int>> list, int count, Random _rand)
        {
			int max = list.Capacity;
			int prob = 0;
			switch (difficulty)
			{
				case "низкий":
				case "easy":
					prob = 10;
					break;
				case "средний":
				case "medium":
					prob = 15;
					break;
				case "высокий":
				case "hard":
					prob = 20;
					break;
			}
			double res = (double)max / (double)prob;
			int chance = _rand.Next(Convert.ToInt32(res * 100));
			
            for (int i = 0; i < max; i++)
            {
                HashSet<int> randomNumbers = new HashSet<int>();
				if (i == chance)
				{
					for (int j = 0; j < count - 1; j++)
						while (!randomNumbers.Add(_rand.Next(count))) ;
				}
				else
				{
					for (int j = 0; j < count; j++)
						while (!randomNumbers.Add(_rand.Next(count))) ;
				}
					           
                list.Add(randomNumbers.ToList());
            }
        }

        public static void Calculate()
        {
			List<List<int>> m_pref_copy = new List<List<int>>();
			foreach(List<int> row in m_pref)
			{
				List<int> temp = new List<int>();
				row.ForEach((item) =>
				{
					temp.Add(item);
				});
				m_pref_copy.Add(temp);
			}

			pairs = new List<int>(m_pref_copy.Count);
			pairs.AddRange(Enumerable.Repeat(-1, pairs.Capacity));
			bool check = true;
			while (check)
			{
				check = false;

				for (int i = 0; i < pairs.Count; i++)
				{
					if (m_pref_copy[i].Count > 0)
					{
						int cur_w = m_pref_copy[i][0];
						if (pairs[i] != cur_w)
						{
							if (w_pref[cur_w].Contains(i))
							{
								if (pairs.Contains(cur_w))
								{
									int competitor = pairs.IndexOf(cur_w);
									if (w_pref[cur_w].IndexOf(competitor) > w_pref[cur_w].IndexOf(i))
									{
										pairs[i] = cur_w;
										pairs[competitor] = -1;
										check = true;
									}
									else
									{
										m_pref_copy[i].RemoveAt(0);
										check = true;
									}
								}
								else
								{
									pairs[i] = cur_w;
									check = true;
								}
							}
							else
							{
								m_pref_copy[i].RemoveAt(0);
								check = true;
							}
						}
					}
				}
			}
		}

		public static bool IsOneNegative(int num)
		{
			return num == -1;
		}

		public static string FindBlock(List<int> answer)
		{
			string blocks = string.Empty;
			for (int i = 0; i < answer.Count; i++)
			{
				if (blocks.Length == 0)
				{
					int temp = m_pref[i].Count;
					if (m_pref[i].Contains(answer[i]))
					{
						temp = m_pref[i].IndexOf(answer[i]);
					}
					for (int j = 0; j < temp; j++)
					{
						if (blocks.Length == 0)
						{
							int cur = m_pref[i].ElementAt(j);
							int cur_pair = answer.IndexOf(cur);
							if (w_pref[cur].Contains(i))
							{
								if (w_pref[cur].IndexOf(i) < w_pref[cur].IndexOf(cur_pair))
								{
									blocks = i.ToString() + cur_pair.ToString();
									break;
								}
							}
						}
					}
				}
			}
			return blocks;
		}
	}

	static class Extensions
	{
		public static IList<T> Clone<T>(this IList<T> listToClone) where T : ICloneable
		{
			return listToClone.Select(item => (T)item.Clone()).ToList();
		}
	}
}
