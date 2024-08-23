import 'dart:convert';

import 'package:ui/models/arrangement_earnings.dart';
import 'package:ui/models/user.dart';
import 'package:ui/models/user_names.dart';
import 'package:ui/models/user_payment.dart';
import 'base_provider.dart';

class UserProvider extends BaseProvider<User, User> {
  UserProvider() : super("User");

  @override
  User fromJson(data) {
    return User.fromJson(data);
  }

  @override
  User fromJsonSearch(data) {
    return User.fromJson(data);
  }

  Future<User?> newPassword(var id) async {
    var url = Uri.parse("$baseUrl$endpoint/new-password/$id");

    Map<String, String>? headers = await createHeaders();
    var response = await http!.put(url, headers: headers);

    if (isValidResponseCode(response)) {
      var data = jsonDecode(response.body);
      return fromJson(data) as User;
    } else {
      return null;
    }
  }

    Future<List<UserNamesResponse>> getUserNamesByIds([dynamic request]) async {
    var url = "$baseUrl$endpoint/names-by-id";
    var uri = Uri.parse(url);

    Map<String, String>? headers = await createHeaders();

    var response =
        await http!.post(uri, headers: headers, body: jsonEncode(request));

    if (isValidResponseCode(response)) {
      var data = jsonDecode(response.body);
      return data
          .map((x) => UserNamesResponse.fromJson(x))
          .cast<UserNamesResponse>()
          .toList();
    } else {
      return [];
    }
  }

  Future updatePassword(String currentPassword, String newPassword) async {
    var url = Uri.parse("$baseUrl$endpoint/update-password");

    Map<String, String>? headers = await createHeaders();

    Map<String, String> body2 = {
      'currentPassword': currentPassword,
      'newPassword': newPassword,
    };

    var response =
        await http!.post(url, headers: headers, body: jsonEncode(body2));

    return response.statusCode == 200;
  }
}