using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;

namespace LiangGeRen.Extension
{
	public static class Extended
	{
		public static void AddRange<T>(this ObservableCollection<T> observableCollection, List<T> list)
		{
			foreach (var item in list)
			{
				observableCollection.Add(item);
			}
		}
	}
}
