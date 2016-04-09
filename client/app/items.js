const intialFoodsState = {
  foods : []
}

const FoodListSuccess = "FOOD_LIST_SUCCESS"

export function listFoods(foods) {
  return {
    type : FoodListSuccess,
    foods : foods
  }
}

export function foodsReducer (state = intialFoodsState, action) {
  if (action.type === FoodListSuccess) {
    return action.foods;
  }
  return state;
}

const intialDrinksState = {
  drinks : []
}

const DrinksListSuccess = "DRINKS_LIST_SUCCESS"

export function listDrinks(drinks) {
  return {
    type : DrinksListSuccess,
    drinks : drinks
  }
}

export function drinksReducer (state = intialDrinksState, action) {
  if (action.type === DrinksListSuccess) {
    return action.drinks;
  }
  return state;
}