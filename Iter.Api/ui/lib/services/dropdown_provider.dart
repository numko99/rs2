import 'package:ui/models/dropdown_model.dart';

import 'base_provider.dart';

class DropdownProvider extends BaseProvider<DropdownModel> {
  DropdownProvider() : super("Dropdown");

  @override
  DropdownModel fromJson(data) {
    return DropdownModel.fromJson(data);
  }
}
