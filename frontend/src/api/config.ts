import { Configuration } from "./runtime.ts";

const baseURL = import.meta.env.VITE_API_URL as string;

const config = new Configuration({
  basePath: baseURL,
});

export default config;
