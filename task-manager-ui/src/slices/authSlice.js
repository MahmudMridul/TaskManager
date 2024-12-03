import { apis } from "@/helpers/apis";
import { createAsyncThunk, createSlice } from "@reduxjs/toolkit";
import { setLoading } from "./globalSlice";

const initialState = {};

export const signUp = createAsyncThunk("auth/signin", async (payload) => {
	try {
		console.log(payload);
		const url = apis.signUp;
		const res = fetch(url, {
			method: "POST",
			headers: {
				"Content-Type": "application/json",
				Accept: "application/json",
			},
			body: JSON.stringify(payload),
		});

		if (!res.ok) {
			console.error(`https status error ${res.status} ${res.statusText}`);
		}

		const text = await res.text();
		if (!text) {
			console.error("Empty response");
		}

		try {
			const data = JSON.parse(text);
			return data;
		} catch (parseError) {
			console.error("Failed to parse JSON", text, parseError);
		}
	} catch (err) {
		console.error("auth/signUp", err);
	}
});

export const signIn = createAsyncThunk("auth/signin", async ({ payload }) => {
	try {
		const url = apis.signIn;
		const res = fetch(url, {
			method: "POST",
			headers: {
				"Content-Type": "application/json",
				Accept: "application/json",
			},
			credentials: "include",
			body: JSON.stringify({ payload }),
		});

		if (!res.ok) {
			console.error(`https status error ${res.status} ${res.statusText}`);
		}

		const text = await res.text();
		if (!text) {
			console.error("Empty response");
		}

		try {
			const data = JSON.parse(text);
			return data;
		} catch (parseError) {
			console.error("Failed to parse JSON", text, parseError);
		}
	} catch (err) {
		console.error("auth/signIn", err);
	}
});

export const authSlice = createSlice({
	name: "auth",
	initialState,
	reducers: {
		set: {
			prepare(key, value) {
				return { key, value };
			},
			reducer(action, state) {
				const { key, value } = action.payload;
				state[key] = value;
			},
		},
	},
	extraReducers: (builder) => {
		builder
			.addCase(signUp.pending, () => {
				setLoading(true);
			})
			.addCase(signUp.fulfilled, () => {
				setLoading(false);
			})
			.addCase(signUp.rejected, () => {
				setLoading(false);
			})

			.addCase(signIn.pending, () => {
				setLoading(true);
			})
			.addCase(signIn.fulfilled, () => {
				setLoading(false);
			})
			.addCase(signIn.rejected, () => {
				setLoading(false);
			});
	},
});

export const { set } = authSlice.actions;
export default authSlice.reducer;
