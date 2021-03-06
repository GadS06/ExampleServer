﻿using System;
using System.Collections.Generic;
using LoreSoft.MathExpressions;

namespace Model
{
	public class BaseCountingMethod
	{
		public InitialData Data { get; set; }
		public List<float> Result { get; set; }

		protected float[,] OldGrid { get; set; }
		protected float[,] NewGrid { get; set; }

		protected MathEvaluator Evaluator { get; set; }

		public BaseCountingMethod()
		{
			Evaluator = new MathEvaluator();

			Evaluator.Variables.Add("x", 0);
			Evaluator.Variables.Add("y", 0);
		}

		// Внутренняя часть ЧМ
		protected void MainCycleInside(int i)
		{
			for (int j = 0 + 1; j < Data.NumOfStepsY - 1; j++)
			{
				float l1 = (OldGrid[i - 1, j] - 2 * OldGrid[i, j] + OldGrid[i + 1, j]) / (Data.StepLengthX * Data.StepLengthX);
				float l2 = (OldGrid[i, j - 1] - 2 * OldGrid[i, j] + OldGrid[i, j + 1]) / (Data.StepLengthY * Data.StepLengthY);
				NewGrid[i, j] = OldGrid[i, j] + Data.StepLengthTime * (l1 + l2);
			}
		}

		// Проверка ввода
		protected void InputCheck()
		{
			if (Data.StepLengthTime <= 0 ||
				Data.NumOfStepsTime <= 0 ||
				Data.StepLengthX <= 0 ||
				Data.NumOfStepsX <= 0 ||
				Data.StepLengthY <= 0 ||
				Data.NumOfStepsY <= 0)
			{
				throw new Exception("Values must be positive.");
			}

			int normalizedPointX = Convert.ToInt32(Data.PointX / Data.StepLengthX);
			int normalizedPointY = Convert.ToInt32(Data.PointY / Data.StepLengthY);

			if (normalizedPointX >= Data.NumOfStepsX ||
			    normalizedPointY >= Data.NumOfStepsY ||
				normalizedPointX < 0 ||
				normalizedPointY < 0)
			{
				throw new Exception("Point is out of the field.");
			}

			Evaluator.Variables["x"] = 0;
			Evaluator.Variables["y"] = 0;
			Evaluator.Evaluate(Data.BorderFunc);
			Evaluator.Evaluate(Data.ZeroFunc);
		}

		// Заполнение начальными значениями
		protected void InitialSet()
		{
			OldGrid = new float[Data.NumOfStepsX, Data.NumOfStepsY];
			NewGrid = new float[Data.NumOfStepsX, Data.NumOfStepsY];

			for (int i = 0; i < Data.NumOfStepsX; i++)
			{
				for (int j = 0; j < Data.NumOfStepsY; j++)
				{
					Evaluator.Variables["x"] = i * Data.StepLengthX;
					Evaluator.Variables["y"] = j * Data.StepLengthY;
					OldGrid[i, j] = (float)Evaluator.Evaluate(Data.ZeroFunc);
				}
			}
		}

		// Заполнение краёв
		protected void EdgesSet()
		{
			for (int i = 0; i < Data.NumOfStepsX; i++)
			{
				Evaluator.Variables["x"] = i * Data.StepLengthX;
				Evaluator.Variables["y"] = 0;
				NewGrid[i, 0] = (float)Evaluator.Evaluate(Data.BorderFunc);

				Evaluator.Variables["x"] = i * Data.StepLengthX;
				Evaluator.Variables["y"] = (Data.NumOfStepsY - 1) * Data.StepLengthY;
				NewGrid[i, Data.NumOfStepsY - 1] = (float)Evaluator.Evaluate(Data.BorderFunc);
			}

			for (int j = 1; j < Data.NumOfStepsY - 1; j++)
			{
				Evaluator.Variables["x"] = 0;
				Evaluator.Variables["y"] = j * Data.StepLengthY;
				NewGrid[0, j] = (float)Evaluator.Evaluate(Data.BorderFunc);

				Evaluator.Variables["x"] = (Data.NumOfStepsX - 1) * Data.StepLengthX;
				Evaluator.Variables["y"] = j * Data.StepLengthY;
				NewGrid[Data.NumOfStepsX - 1, j] = (float)Evaluator.Evaluate(Data.BorderFunc);
			}
		}
	}
}
