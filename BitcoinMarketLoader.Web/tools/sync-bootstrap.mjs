import { cp, mkdir, rm } from "node:fs/promises";

const sourceRoot = new URL("../node_modules/bootstrap/", import.meta.url);
const targetRoot = new URL("../wwwroot/lib/bootstrap/", import.meta.url);

await mkdir(targetRoot, { recursive: true });

for (const directory of ["dist", "scss"]) {
  const target = new URL(`${directory}/`, targetRoot);

  await rm(target, { recursive: true, force: true });
  await cp(new URL(`${directory}/`, sourceRoot), target, { recursive: true });
}

await cp(new URL("LICENSE", sourceRoot), new URL("LICENSE", targetRoot));
