export default function ShoppingList({ shoppingList }) {
  if (!shoppingList || shoppingList.length === 0) {
    return <p>No shopping list available.</p>;
  }

  // Group items by category
  const grouped = {};
  for (const item of shoppingList) {
    const category = item.category || 'Other';
    if (!grouped[category]) {
      grouped[category] = [];
    }
    grouped[category].push(item);
  }

  const categories = Object.keys(grouped).sort();

  const totalCost = shoppingList.reduce(
    (sum, item) => sum + (item.estimatedCost || 0),
    0
  );

  return (
    <div className="shopping-list">
      <h3>Shopping List</h3>
      {categories.map((category) => (
        <div key={category} className="shopping-category">
          <h4 className="category-name">{category}</h4>
          <ul className="shopping-items">
            {grouped[category].map((item, index) => (
              <li key={index} className="shopping-item">
                <span className="item-name">{item.ingredientName}</span>
                <span className="item-qty">
                  {item.totalQuantity} {item.unit}
                </span>
                {item.estimatedCost != null && (
                  <span className="item-cost">
                    ${item.estimatedCost.toFixed(2)}
                  </span>
                )}
              </li>
            ))}
          </ul>
        </div>
      ))}
      <div className="shopping-total">
        <strong>Estimated Total:</strong> ${totalCost.toFixed(2)}
      </div>
    </div>
  );
}
