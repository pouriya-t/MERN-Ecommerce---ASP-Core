import axios from "axios";
import {
  ADD_TO_CART,
  REMOVE_ITEM_CART,
  SAVE_SHIPPING_INFO,
  CLEAR_CART,
} from "../constants/cartConstants";

export const addItemToCart = (id, quantity) => async (dispatch, getState) => {
  const { data } = await axios.get(`/api/v1/product/${id}`);
  dispatch({
    type: ADD_TO_CART,
    payload: {
      product: data.product.id,
      name: data.product.name,
      price: data.product.price,
      image: data.product.images[0].url,
      stock: data.product.stock,
      quantity,
    },
  });

  localStorage.setItem("cartItems", JSON.stringify(getState().cart.cartItems));
};

export const removeItemFromCart = (id) => async (dispatch, getState) => {
  dispatch({
    type: REMOVE_ITEM_CART,
    payload: id,
  });

  localStorage.setItem("cartItems", JSON.stringify(getState().cart.cartItems));
};

export const clearCart = () => async (dispatch) => {
  dispatch({
    type: CLEAR_CART,
  });

  localStorage.removeItem("cartItems");
};

export const saveShippingInfo = (data) => async (dispatch, getState) => {
  dispatch({
    type: SAVE_SHIPPING_INFO,
    payload: data,
  });
  /// aaa
  localStorage.setItem("shippingInfo", JSON.stringify(data));
};
