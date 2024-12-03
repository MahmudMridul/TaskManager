import { configureStore } from "@reduxjs/toolkit";
import { authSlice } from "./slices/authSlice";
import { globalSlice } from "./slices/globalSlice";

export const store = configureStore({
   reducer: {
      global: globalSlice.reducer,
      auth: authSlice,
   },
});

// export const { setLoading } = globalSlice.actions;
