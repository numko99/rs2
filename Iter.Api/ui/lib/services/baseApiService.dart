import 'dart:convert';
import 'package:http/http.dart' as http;
import 'package:shared_preferences/shared_preferences.dart';
import 'package:ui/apiConfig.dart';

class BaseApiService {
  String? authToken;

  Future<String?> getAuthToken() async {
    if (authToken != null) {
      return authToken;
    }

    SharedPreferences prefs = await SharedPreferences.getInstance();
    authToken = prefs.getString('authToken');
    return authToken;
  }

  Future<void> setAuthToken(String token) async {
    authToken = token;
    SharedPreferences prefs = await SharedPreferences.getInstance();
    await prefs.setString('authToken', token);
  }

  Future<void> clearAuthToken() async {
    SharedPreferences prefs = await SharedPreferences.getInstance();
    await prefs.remove('authToken');
    authToken = null;
  }

  Future<http.Response> get(String endpoint) async {
    String url = '$ApiConfig.baseUrl/$endpoint';
    String? token = await getAuthToken();

    var response = await http.get(Uri.parse(url), headers: {
      'Content-Type': 'application/json',
      'Authorization': 'Bearer $token',
    });

    return response;
  }

  Future<http.Response> post(String endpoint,
      {required Map<String, dynamic> body}) async {
    String url = '$ApiConfig.baseUrl/$endpoint';
    String? token = await getAuthToken();

    var response = await http.post(Uri.parse(url),
        headers: {
          'Content-Type': 'application/json',
          'Authorization': 'Bearer $token',
        },
        body: jsonEncode(body));

    return response;
  }
}
