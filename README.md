# kale-app
An app that helps users coordinate their grocery shopping and meal plan, optimizing for price, healthiness, ease, and deliciousness.

- users provide details on each member of their household and their dietary needs, including:
  - height, weight, sex, activity level, goals, etc. - everything needed to determine daily calorie goal
  - allergies
  - likes
  - dislikes
- App plans out meals for everyone for one week. User can veto meals they don't want and app will recalculate the week with a new meal to make sure everyone still gets their required nutrients
- Final output is a beautiful PDF or HTML page that includes a shopping list and all the recipes for the week.
  - User should basically be able to sign up for the app on sunday, input their household information, figure out the meal plan for the week, get their PDF, then shop for the week. Throughout the week they will use the recipes to cook for their household. Next sunday, they do it again. Nothing ever goes bad, their grocery bill goes down, the household is eating better than ever, and everyone's diet is extremely healthy.
- app is opinionated; basically follows the Mediterranian Diet, and always yields dinners portioned so that everyone has leftovers for lunch the next day.
- Uses databases of ingredients (groceries) and recipes to plan out meals for one week that
  - meet everyone's micro and macronutrient needs REALLY well according to proven nutritional scientific consensus
  - everyone enjoys
  - are reasonably easy to cook
  - are properly portioned - no waste
  - optimize for cost of groceries
- Ingredient database should have very detailed nutritional data for all ingredients you'd expect to find in a grocery store
- Recipe database should have well-structured information about the ingredients that go into it and come out of it (for instance, chicken noodle soup requires raw carrots to cook, but when you eat it, those carrots are cooked and have a different micronutrient profile)
