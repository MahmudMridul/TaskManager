import { createSlice } from "@reduxjs/toolkit";

const initialState = {
   loading: false,
};

export const globalSlice = createSlice({
   name: "auth",
   initialState,
   reducers: {
      setLoading(state, action) {
         state.loading = action.payload;
      },
   },
});

export const { setLoading } = globalSlice.actions;
export default globalSlice.reducer;
