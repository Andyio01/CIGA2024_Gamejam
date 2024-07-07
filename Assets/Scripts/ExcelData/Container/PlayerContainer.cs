/* ------------------------------------
/* Title: Player 数据结构容器类
/* Creation Time: 2024/7/7 9:52:21
/* Description: It is an automatically generated data structure class.
/* 描述: 自动生成的 Player 数据结构容器类。
/* 此文件为自动生成，请尽量不要修改，重新生成将会覆盖原有修改！！！
--------------------------------------- */

using System.Collections.Generic;

public class PlayerContainer 
{
	private Dictionary<int, Player> dataDic = new Dictionary<int, Player>();

	public Player GetData(int key)
	{
		if (dataDic.TryGetValue(key, out Player data))
		{
			return data;
		}
		return null;
	}
}
