import 'dart:convert';

import 'package:ui/models/dropdown_model.dart';
import 'package:ui/models/employee_arrangement.dart';
import 'package:ui/models/user.dart';
import 'base_provider.dart';

class EmployeeArrangmentProvider extends BaseProvider<EmployeeArrangment, EmployeeArrangment> {
  EmployeeArrangmentProvider() : super("EmployeeArrangment");

  @override
  EmployeeArrangment fromJson(data) {
    return EmployeeArrangment.fromJson(data);
  }

  @override
  EmployeeArrangment fromJsonSearch(data) {
    return EmployeeArrangment.fromJson(data);
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

  Future<List<DropdownModel>?> getAvailableEmployeeArrangments(var id, var dateFrom, var dateTo) async {
    var url = Uri.parse("$baseUrl$endpoint/available-guides?arrangementId=$id&dateFrom=$dateFrom&dateTo=$dateTo");

    Map<String, String>? headers = await createHeaders();
    var response = await http!.get(url, headers: headers);

    if (isValidResponseCode(response)) {
      var data = jsonDecode(response.body);
      return data.map((x) => DropdownModel.fromJson(x)).cast<DropdownModel>().toList();
    } else {
      return null;
    }
  }
}