enum Roles {
  admin,
  coordinator,
  touristGuide,
  client,
}

class RoleEnumManager {
  static Roles getRoleById(int id) {
    if (id == 1) {
      return Roles.admin;
    }
    if (id == 2) {
      return Roles.coordinator;
    }
    if (id == 3) {
      return Roles.touristGuide;
    }
    if (id == 4) {
      return Roles.client;
    }

    return Roles.admin;
  }

  static String getRoleNamesById(int id) {
    if (id == 1) {
      return "Administrator";
    }
    if (id == 2) {
      return "Koordinator";
    }
    if (id == 3) {
      return "Turistički vodič";
    }
    if (id == 4) {
      return "Klijent";
    }

    return "Administrator";
  }
}
