﻿using System;

public static class Extensions
{
	public static bool IsNullOrEmpty(this Array array)
	{
		return array == null || array.Length == 0;
	}
}