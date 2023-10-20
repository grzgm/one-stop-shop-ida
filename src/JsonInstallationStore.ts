import {
  Installation,
  InstallationStore,
  InstallationQuery,
} from "@slack/bolt";
import { Logger } from "@slack/bolt";
import fs from "fs";

export default class JsonInstallationStore implements InstallationStore {
  private baseDir = "installation.json";
  async storeInstallation(installation: Installation): Promise<void> {
    const json = JSON.stringify(installation);
    fs.writeFile(this.baseDir, json, "utf8", (err) => {
      if (err) throw err;
      console.log("complete");
    });
  }
  async fetchInstallation(
    query: InstallationQuery<boolean>,
    logger?: Logger
  ): Promise<Installation> {
    const data = fs.readFileSync(this.baseDir, "utf8");
    const installation = <Installation> JSON.parse(data);
    return installation;
  }
  async deleteInstallation(
    query: InstallationQuery<boolean>,
    logger?: Logger
  ): Promise<void> {
    fs.readFile(this.baseDir, "utf8", (err, data) => {
      if (err) {
        console.log(err);
      } else {
        let obj = JSON.parse(data); //now it an object
        obj.table.push({ id: 2, square: 3 }); //add some data
        JSON.stringify(obj); //convert it back to json
        fs.writeFile(this.baseDir, "", "utf8", (err) => {
          if (err) throw err;
          console.log("complete");
        });
      }
    });
  }
}
//# sourceMappingURL=file-store.d.ts.map
