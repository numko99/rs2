import 'dart:convert';
import 'package:flutter/foundation.dart';
import 'package:flutter/material.dart';
import 'package:http/http.dart' as http;
import 'package:iter_mobile/models/auth_request.dart';
import 'package:iter_mobile/pages/login.dart';
import 'package:iter_mobile/providers/auth_storage_provider.dart';
import '../apiConfig.dart';

class AuthProvider with ChangeNotifier {
  AuthProvider();
  String baseUrl = const String.fromEnvironment('BASE_URL', defaultValue: ApiConfig.baseUrl);

  Future<bool> loginUserAsync(AuthRequest authRequest) async {
    final Uri url =
        Uri.parse("${baseUrl}/auth/login");

    Map<String, String> headers = {"Content-type": "application/json"};
    http.Response? response = null;;
    try {

      http.Response response = await http.post(
        url,
        headers: headers,
        body: jsonEncode(authRequest),
      );


      if (response?.statusCode == 200) {
        final Map<String, dynamic> responseData = jsonDecode(response!.body!);
        final String token = responseData['token'];
        final int role = responseData['role'] as int;
        final String id = responseData['id'] ;
        final String? agencyId =
            responseData['agencyId'] == "" ? null : responseData['agencyId'];
        AuthStorageProvider.saveToken(token);
        AuthStorageProvider.saveAuthData(role, agencyId, id);

        return true;
      } else {
        return false;
      }
    } catch (error) {
      return false;
    }
  }

  Future<Map<bool, String>> registerUserAsync(
      Map<String, dynamic> request) async {
    final Uri url =
        Uri.parse("${baseUrl}/auth/register");

    try {
      final http.Response response = await http.post(
        url,
        headers: {
          'accept': '*/*',
          'Content-Type': 'application/json',
        },
        body: jsonEncode(request),
      );

      if (response.statusCode == 201) {
        return {true: ""};
      }

      if (response.statusCode == 409) {
        return {false: "DuplicateEmail"};
      }

      if (response.statusCode == 400) {
        return {false: "BadRequest"};
      }
    } catch (error) {
      return {false: error.toString()};
    }

    return {false: ""};
  }

  Future resendToken(String email) async {
    final Uri url = Uri.parse(
        "${baseUrl}/auth/resend-token?email=$email");

    try {
      final response = await http.get(url, headers: {
        'Content-Type': 'application/json',
      });

      if (response.statusCode == 200) {
        return {true: ""};
      }

      if (response.statusCode == 401) {
        return {false: "DuplicateEmail"};
      }

      if (response.statusCode == 400) {
        return {false: "BadRequest"};
      }    } catch (error) {
      print("An error occurred during token validation: $error");
      return false;
    }
  }

  Future<bool> verifyEmailVerificationToken(
      Map<String, dynamic> request) async {
    final Uri url = Uri.parse(
        "${baseUrl}/auth/verify-email-token");

    try {
      final http.Response response = await http.post(
        url,
        headers: {
          'accept': '*/*',
          'Content-Type': 'application/json',
        },
        body: jsonEncode(request),
      );
      return response.statusCode == 200;
    } catch (error) {
      return false;
    }
  }

  Future<int> sendForgotPasswordToken(String email) async {
    final Uri url =
        Uri.parse("${baseUrl}/auth/send-forgot-password-token?email=$email");

    try {
      final http.Response response = await http.get(
        url,
        headers: {
          'accept': '*/*',
          'Content-Type': 'application/json',
        },
      );
      return response.statusCode;
    } catch (error) {
      print('Error sending forgot password token: $error');
      return 400;
    }
  }

  Future<bool> verifyForgotPasswordToken(String email, String token) async {
    final Uri url =
        Uri.parse("${baseUrl}/auth/verify-forgot-password-token?email=$email&token=$token");

    try {
      final http.Response response = await http.get(
        url,
        headers: {
          'accept': '*/*',
          'Content-Mane': 'application/json',
        },
      );
      return response.statusCode == 200;
    } catch (error) {
      print('Error verifying forgot password token: $error');
      return false;
    }
  }

  Future<bool> resetPassword(String email, String password) async {
    final Uri url =
        Uri.parse("${baseUrl}/auth/reset-password");

    try {
      final http.Response response = await http.post(
        url,
        headers: {
          'accept': '*/*',
          'Content-Type': 'application/json',
        },
        body: jsonEncode({'email': email, 'password': password}),
      );

      return response.statusCode == 200;
    } catch (error) {
      return false;
    }
  }

  Future logoutUserAsync(context) async {
    AuthStorageProvider.deleteToken();
    Navigator.of(context)
        .pushReplacement(MaterialPageRoute(builder: (context) => Login()));
  }
}
