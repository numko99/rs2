import 'dart:convert';
import 'package:flutter/foundation.dart';
import 'package:http/http.dart' as http;
import 'package:iter_mobile/models/auth_request.dart';
import 'package:iter_mobile/providers/token_storage_provider.dart';
import '../apiConfig.dart';

class AuthProvider with ChangeNotifier {
  AuthProvider();

  Future<bool> loginUserAsync(AuthRequest authRequest) async {
    final Uri url =
        Uri.parse("${ApiConfig.baseUrl}/api/userauthentication/login");
        
    Map<String, String> headers = {"Content-type": "application/json"};
    try {
      final http.Response response = await http.post(
        url,
        headers: headers,
        body: jsonEncode(authRequest),
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
