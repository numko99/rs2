import 'package:iter_mobile/models/employee_arrangement.dart';

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
}