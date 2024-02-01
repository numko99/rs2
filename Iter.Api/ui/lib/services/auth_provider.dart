import 'dart:convert';
import 'package:flutter/foundation.dart';
import 'package:http/http.dart' as http;
import 'package:ui/services/token_storage_provider.dart';
import '../apiConfig.dart';

class AuthProvider with ChangeNotifier {
  AuthProvider();

  Future<bool> loginUserAsync(String username, String password) async {
    final Uri url =
        Uri.parse("${ApiConfig.baseUrl}/api/userauthentication/login");

    try {
      final http.Response response = await http.post(
        url,
        headers: {
          'accept': '*/*',
          'Content-Type': 'application/json',
        },
        body: jsonEncode({
          "userName": username,
          "password": password,
        }),
      );

      if (response.statusCode == 200) {
        final Map<String, dynamic> responseData = jsonDecode(response.body);
        final String token = responseData['token'];
        TokenStorageProvider.saveToken(token);

        return true;
      } else {
        return false;
      }
    } catch (error) {
      return false;
    }
  }
}
