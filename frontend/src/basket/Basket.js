export const addToBasket = (id) => {
  const basket = getBasketItems();
  const exists = basket.filter((x) => x === id);
  if (exists.length === 0) {
    basket.push(id);
    localStorage.setItem("basket", JSON.stringify(basket));
  }
};

export const getBasketItems = () => {
  let basketItems = JSON.parse(localStorage.getItem("basket"));
  if (basketItems == null) {
    basketItems = [];
  }
  return basketItems;
};

export const removeFromBasket = (id) => {
  let basket = getBasketItems();
  basket = basket.filter((x) => x !== id);
  localStorage.setItem("basket", JSON.stringify(basket));
};

export const clearBasket = () => {
  localStorage.removeItem("basket");
};
