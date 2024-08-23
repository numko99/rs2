import 'package:shared_preferences/shared_preferences.dart';
import 'package:ui/enums/roles.dart';

class AuthStorageProvider {
  static String? _authToken;
  static final Map<String, dynamic> _authData = {
    "role": null,
    "agencyId": null
  };

  static Future<void> saveToken(String token) async {
    _authToken = token;
    SharedPreferences prefs = await SharedPreferences.getInstance();
    await prefs.setString('authToken', token);
  }

  static Future<String?> readToken() async {
    if (_authToken != null) {
      return _authToken;
    }

    SharedPreferences prefs = await SharedPreferences.getInstance();
    _authToken = prefs.getString('authToken');
    return _authToken;
  }

  static Future<void> deleteToken() async {
    SharedPreferences prefs = await SharedPreferences.getInstance();
    await prefs.remove('authToken');
    _authToken = null;
  }

  static saveAuthData(int role, String? agencyId, String? id) {
    var index = role - 1;
    _authData["role"] = Roles.values[index];
    _authData["agencyId"] = agencyId;
    _authData["id"] = id;
  }

  static Map<String, dynamic>? getAuthData() {
    return _authData;
  }
}
