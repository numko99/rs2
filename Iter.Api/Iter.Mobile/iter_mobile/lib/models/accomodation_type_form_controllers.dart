import 'package:flutter/material.dart';

class AccomodationTypeFormControllers {
  List<TextEditingController> accomodationTypes = [];
  List<TextEditingController> accomodationTypePrices = [];
  List<String?> accomodationTypeIds = [];

  void addArrangementPrices() {
    accomodationTypes.add(TextEditingController());
    accomodationTypePrices.add(TextEditingController());
    accomodationTypeIds.add(null);
  }
}
