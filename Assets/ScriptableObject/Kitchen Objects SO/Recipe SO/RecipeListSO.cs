using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RecipeListSO : ScriptableObject
{
	[SerializeField] private List<RecipeSO> recipeSOList;
	public List<RecipeSO> RecipeSOList {  get { return recipeSOList; } }
}
