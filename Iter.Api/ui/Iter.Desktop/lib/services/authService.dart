import 'dart:convert';
import 'package:http/http.dart' as http;
import 'package:injectable/injectable.dart';

import '../apiConfig.dart';
import 'baseApiService.dart';

@injectable
class AuthService {
  final BaseApiService _baseApiService;

  AuthService(this._baseApiService);

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

        await _baseApiService.setAuthToken(token);
        return true;
      } else {
        return false;
      }
    } catch (error) {
      return false;
    }
  }
}
