import 'package:shared_preferences/shared_preferences.dart';

class TokenStorageProvider {
  static String? _authToken;

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
}
