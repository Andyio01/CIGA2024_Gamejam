/* ------------------------------------
/* Title: Global 数据结构容器类
/* Creation Time: 2024/7/7 10:38:43
/* Description: It is an automatically generated data structure class.
/* 描述: 自动生成的 Global 数据结构容器类。
/* 此文件为自动生成，请尽量不要修改，重新生成将会覆盖原有修改！！！
--------------------------------------- */

using System.Collections.Generic;

public class GlobalContainer 
{
	private Dictionary<int, Global> dataDic = new Dictionary<int, Global>();

	public Global GetData(int key)
	{
		if (dataDic.TryGetValue(key, out Global data))
		{
			return data;
		}
		return null;
	}
}
